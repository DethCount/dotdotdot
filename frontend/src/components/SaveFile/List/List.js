export default {
  name: 'SaveFileList',
  data () {
    return {
      items: null,
      loaded: false
    }
  },
  created () {
    this.$store.dispatch('loadSaveFileList')
    this.$store.watch(
      (state, getters) => state.saveFileListLoaded,
      (newValue, oldValue) => {
        if (newValue === true) {
          this.items = this.$store.state.saveFileList.files
          this.loaded = true
        }
      }
    )
  }
}
