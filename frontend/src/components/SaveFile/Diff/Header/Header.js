import { defineAsyncComponent } from 'vue'

export default {
  name: 'SaveFileHeaderDiff',
  components: {
    Tree: defineAsyncComponent(() => import('@/components/Tree/Tree/Tree.vue')),
    Spinner: defineAsyncComponent(() => import('@/components/Spinner/Spinner.vue'))
  },
  props: ['filename1', 'filename2'],
  data () {
    return {
      header: null,
      loaded: false
    }
  },
  created () {
    this.$store.dispatch(
      'loadSaveFileHeaderDiff',
      {
        filename1: this.filename1,
        filename2: this.filename2
      }
    )
    this.$store.watch(
      (state, getters) => {
        return state.saveFileHeaderDiff[this.filename2]
      },
      (newValue, oldValue) => {
        if (newValue !== undefined &&
          Object.prototype.hasOwnProperty.call(newValue, this.filename1)
        ) {
          this.header = newValue[this.filename1].header
          this.loaded = true
        }
      }
    )
  }
}
