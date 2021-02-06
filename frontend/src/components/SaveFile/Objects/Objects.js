export default {
  name: 'SaveFileObjects',
  props: ['filename'],
  data () {
    return {
      objects: null,
      objectsLoaded: false,
      properties: null,
      propertiesLoaded: false
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
          this.objects = newValue.objects
          this.objectsLoaded = true
          this.$store.dispatch('loadSaveFileProperties', { filename: this.filename })
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
