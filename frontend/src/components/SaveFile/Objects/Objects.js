import Tree from '@/components/Tree/Tree/Tree.vue'

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
          this.$store.dispatch('loadSaveFileProperties', { filename: this.filename })
        }
      }
    )

    this.$store.watch(
      (state, getters) => {
        return state.saveFileObjectsTree[this.filename]
      },
      (newValue, oldValue) => {
        if (newValue !== null) {
          this.tree = newValue
          this.objectsLoaded = true
        }
      }
    )
  }
}
