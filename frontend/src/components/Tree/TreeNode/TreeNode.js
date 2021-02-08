import { defineAsyncComponent } from 'vue'
import * as d3 from 'd3'

export default {
  name: 'TreeNode',
  components: {
    Tree: defineAsyncComponent(() => import('@/components/Tree/Tree/Tree.vue'))
  },
  props: ['node'],
  data () {
    return {
      showBody: false
    }
  },
  computed: {
    showValues () {
      console.log(this)
      return this.node[1].values ||
        (
          Object.prototype.hasOwnProperty
            .call(this.node[1], 'value') &&
          !this.isRowableValue(this.node[1].value)
        )
    },
    values () {
      const values = this.node[1].type === 'MapProperty'
        ? Object.values(this.node[1].value)
        : [].concat(
          this.node[1].value
            ? [this.node[1].value]
            : this.node[1].values
        )

      return new Map([['Values', values]])
    },
    showValueRow () {
      return Object.prototype.hasOwnProperty.call(this.node[1], 'value') &&
        this.isRowableValue(this.node[1].value)
    },
    showPropertyRow () {
      return this.node[1].property === undefined ||
        this.node[1].property === null ||
        typeof this.node[1].property !== 'object' ||
        !Object.prototype.hasOwnProperty.call(this.node[1].property, 'name')
    }
  },
  methods: {
    isRowableValue (value) {
      return value === null ||
        value === undefined ||
        typeof value === 'string' ||
        typeof value !== 'object'
    },
    showTableRow (key, value) {
      console.log(key, value, this.showValueRow, this.isRowableValue(value))
      return (
        key !== 'properties' &&
        key !== 'property' &&
        key !== 'values' &&
        key !== 'value'
      ) || (
        key === 'value' &&
        this.showValueRow
      ) || (
        key === 'property' &&
        this.showPropertyRow
      )
    },
    hasProperties () {
      return (
        Object.prototype.hasOwnProperty.call(this.node[1], 'property') &&
        !this.showPropertyRow
      ) || (
        this.node[1].properties !== undefined &&
        this.node[1].properties !== null &&
        (
          this.node[1].properties instanceof Map ||
          this.node[1].properties instanceof Array
        )
      )
    },
    getProperties () {
      return this.node[1].property !== undefined
        ? d3.index([this.node[1].property], d => d.name)
        : (
          this.node[1].properties instanceof Map
            ? this.node[1].properties
            : d3.index(this.node[1].properties, d => d.name)
        )
    }
  }
}
