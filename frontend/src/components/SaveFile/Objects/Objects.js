import { defineAsyncComponent } from 'vue'

export default {
  name: 'SaveFileObjects',
  components: {
    Tree: defineAsyncComponent(() => import('@/components/Tree/Tree/Tree.vue')),
    Spinner: defineAsyncComponent(() => import('@/components/Spinner/Spinner.vue'))
  },
  props: ['filename'],
  data () {
    return {
      loaded: false,
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
          this.loaded = true
        }
      }
    )
  }
}
