import { defineAsyncComponent } from 'vue'

export default {
  name: 'SaveFileHeader',
  components: {
    Spinner: defineAsyncComponent(() => import('@/components/Spinner/Spinner.vue'))
  },
  props: ['filename'],
  data () {
    return {
      header: null,
      loaded: false
    }
  },
  mounted () {
    this.$store.dispatch('loadSaveFileHeader', { filename: this.filename })
    this.$store.watch(
      (state, getters) => {
        return state.saveFileHeaders[this.filename]
      },
      (newValue, oldValue) => {
        if (newValue !== null) {
          this.header = newValue.header
          this.loaded = true
        }
      }
    )
  }
}
