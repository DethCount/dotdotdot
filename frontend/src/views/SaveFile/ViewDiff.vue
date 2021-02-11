<template>
  <div class="save-file-read">
    <HeaderDiff :filename1="filename1" :filename2="filename2" />
    <ObjectsDiff :filename1="filename1" :filename2="filename2" />
  </div>
</template>

<script>
import { defineAsyncComponent } from 'vue'

export default {
  name: 'SaveFileDiffView',
  components: {
    HeaderDiff: defineAsyncComponent(() => import('@/components/SaveFile/Diff/Header/Header.vue')),
    ObjectsDiff: defineAsyncComponent(() => import('@/components/SaveFile/Diff/Objects/Objects.vue'))
  },
  props: ['filename1', 'filename2']
}
</script>
