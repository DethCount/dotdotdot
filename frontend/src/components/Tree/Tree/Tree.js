import { defineAsyncComponent } from 'vue'

export default {
  name: 'Tree',
  props: ['nodes', 'nodeComponent', 'scalarComponent'],
  components: {
    TreeNode: defineAsyncComponent(
      () => import('@/components/Tree/TreeNode/TreeNode.vue')
    )
  },
  data () {
    return {}
  },
  created () {
    // console.log('created tree', this.nodeComponent)
  }
}
