<template>
  <div class="save-file-read">
    <SaveFileHeader v-bind:filename="filename"/>
    <SaveFileObjects v-bind:filename="filename"/>
  </div>
</template>

<script>
// @ is an alias to /src
import SaveFileHeader from '@/components/SaveFile/Header/Header.vue'
import SaveFileObjects from '@/components/SaveFile/Objects/Objects.vue'

export default {
  name: 'SaveFileRead',
  props: ['filename'],
  components: {
    SaveFileHeader,
    SaveFileObjects
  }
}
</script>
