export default {
  name: 'SaveFileDiff',
  props: ['filename1', 'filename2'],
  data () {
    return {
    }
  },
  mounted () {
    this.$store.dispatch(
      'loadSaveFileDiff',
      {
        filename1: this.filename1,
        filename2: this.filename2
      }
    )
  }
}
