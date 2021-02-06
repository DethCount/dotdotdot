export default {
  name: 'SaveFiles',
  data () {
    return {
      items: null,
      loaded: false
    }
  },
  mounted () {
    console.log('mounted', this.$store)
    this.$store.dispatch('loadSaveFiles')
    this.$store.watch(
      (state, getters) => state.saveFilesLoaded,
      (newValue, oldValue) => {
        console.log('check', newValue, oldValue)
        if (newValue === true) {
          this.items = this.$store.state.saveFiles.files
          this.loaded = true
        }
      }
    )
  }
}
