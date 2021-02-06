<template>
  <div class="home">
    <SaveFiles />
  </div>
</template>

<script>
// @ is an alias to /src
import SaveFiles from '@/components/SaveFiles/SaveFiles.vue'

export default {
  name: 'Home',
  components: {
    SaveFiles
  }
}
</script>
