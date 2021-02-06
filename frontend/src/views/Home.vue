<template>
  <div class="home">
    <SaveFileList />
  </div>
</template>

<script>
// @ is an alias to /src
import SaveFileList from '@/components/SaveFile/List/List.vue'

export default {
  name: 'Home',
  components: {
    SaveFileList
  }
}
</script>
