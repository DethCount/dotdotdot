import { defineAsyncComponent } from 'vue'

export default {
  name: 'SaveFileList',
  components: {
    Spinner: defineAsyncComponent(() => import('@/components/Spinner/Spinner.vue'))
  },
  data () {
    return {
      items: null,
      loaded: false,
      firstSelectedRow: null,
      secondSelectedRow: null
    }
  },
  computed: {
    showActions () {
      return this.showDiffBtn
    },
    showDiffBtn () {
      return this.secondSelectedRow !== null
    }
  },
  methods: {
    onRowClick (item) {
      if (this.firstSelectedRow !== null) {
        if (this.secondSelectedRow !== null) {
          this.firstSelectedRow = this.secondSelectedRow
        }
        this.secondSelectedRow = item.filename
      } else {
        this.firstSelectedRow = item.filename
      }

      if (this.firstSelectedRow === this.secondSelectedRow) {
        this.firstSelectedRow = null
        this.secondSelectedRow = null
      }
    },
    isSelectedFirst (item) {
      return item.filename !== null &&
          item.filename === this.firstSelectedRow
    },
    isSelectedSecond (item) {
      return item.filename !== null &&
        item.filename === this.secondSelectedRow
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
