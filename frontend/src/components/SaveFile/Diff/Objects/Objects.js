import { defineAsyncComponent } from 'vue'

export default {
  name: 'SaveFileObjectsDiff',
  components: {
    Tree: defineAsyncComponent(() => import('@/components/Tree/Tree/Tree.vue')),
    Spinner: defineAsyncComponent(() => import('@/components/Spinner/Spinner.vue'))
  },
  props: ['filename1', 'filename2'],
  data () {
    return {
      tree: null,
      objectsLoaded: false
    }
  },
  mounted () {
    this.$store.dispatch(
      'loadSaveFileObjectsDiff',
      {
        filename1: this.filename1,
        filename2: this.filename2
      }
    )

    this.$store.watch(
      (state, getters) => {
        return state.saveFileObjectsDiff[this.filename2]
      },
      (newValue, oldValue) => {
        if (newValue !== null && Object.prototype.hasOwnProperty.call(newValue, this.filename1)) {
          this.$store.dispatch(
            'loadSaveFilePropertiesDiff',
            {
              filename1: this.filename1,
              filename2: this.filename2
            }
          )
        }
      }
    )

    this.$store.watch(
      (state, getters) => {
        return state.saveFileObjectsDiffTree[this.filename2]
      },
      (newValue, oldValue) => {
        if (newValue !== null &&
          Object.prototype.hasOwnProperty.call(newValue, this.filename1)
        ) {
          this.tree = newValue[this.filename1]
          this.objectsLoaded = true
        }
      }
    )
  }
}
