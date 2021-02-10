<template>
    <Diff :filename1="filename1" :filename2="filename2"></Diff>
</template>

<script>
import { defineAsyncComponent } from 'vue'

export default {
  name: 'SaveFileDiffView',
  components: {
    Diff: defineAsyncComponent(() => import('@/components/SaveFile/Diff/Diff.vue'))
  },
  props: ['filename1', 'filename2']
}
</script>
