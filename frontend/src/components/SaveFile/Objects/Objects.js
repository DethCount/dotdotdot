import * as d3 from 'd3'
import Tree from '@/components/Tree/Tree/Tree.vue'

function buildTree (objects) {
  const objectsByPathAndType = d3.rollup(
    objects,
    (v) => v.reduce((t, d) => {
      return {
        instanceName: d.id.instanceName,
        ...d.value
      }
    }),
    d => d.id.rootObject,
    d => d.value.type,
    d => d.typePath.split('/')[1],
    d => d.typePath.split('/')
      .reduce((a, v, i) =>
        i > 1
          ? a + (a.length > 0 ? '/' : '') + v
          : ''
      )
  )
  console.log('group', objectsByPathAndType, objectsByPathAndType.entries())

  return objectsByPathAndType
}

export default {
  name: 'SaveFileObjects',
  components: {
    Tree
  },
  props: ['filename'],
  data () {
    return {
      objects: null,
      objectsLoaded: false,
      properties: null,
      propertiesLoaded: false,
      tree: null
    }
  },
  mounted () {
    this.$store.dispatch('loadSaveFileObjects', { filename: this.filename })
    this.$store.watch(
      (state, getters) => {
        return state.saveFileObjects[this.filename]
      },
      (newValue, oldValue) => {
        if (newValue !== null) {
          // this.objects = newValue.objects
          this.tree = buildTree(newValue.objects)
          this.objectsLoaded = true

          // this.$store.dispatch('loadSaveFileProperties', { filename: this.filename })
        }
      }
    )
    this.$store.watch(
      (state, getters) => {
        return state.saveFileProperties[this.filename]
      },
      (newValue, oldValue) => {
        if (newValue !== null) {
          this.properties = newValue.properties
          this.propertiesLoaded = true
        }
      }
    )
  }
}
