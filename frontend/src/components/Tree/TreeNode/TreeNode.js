import { defineAsyncComponent } from 'vue'

export default {
  name: 'TreeNode',
  components: {
    Tree: defineAsyncComponent(() => import('@/components/Tree/Tree/Tree.vue'))
  },
  props: ['node'],
  data () {
    return {
      showBody: false
    }
  },
  created () {
    console.log('created tree node', this.node)
  }
}
