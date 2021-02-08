import { defineAsyncComponent } from 'vue'

export default {
  name: 'Tree',
  props: ['nodes'],
  components: {
    TreeNode: defineAsyncComponent(() => import('@/components/Tree/TreeNode/TreeNode.vue'))
  },
  data () {
    return {}
  },
  created () {
    // console.log('created tree', this.nodes)
  }
}
