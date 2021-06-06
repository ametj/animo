<template>
  <div class="about">
    <h1>This is an about page</h1>
    <HelloI18n />
  </div>
</template>

<script>
// @ is an alias to /src
import HelloI18n from '@/components/HelloI18n.vue'

export default {
  components: {
    HelloI18n
  }
}
</script>
