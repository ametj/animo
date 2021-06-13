<template>
  <div>
    <p>{{ t('text') }}</p>
    <p>{{ t('message') }}</p>
  </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue'
import { useI18n } from 'vue-i18n'

export default defineComponent({
  name: 'HelloI18n',
  setup() {
    const { t } = useI18n({
      useScope: 'global'
    })

    // Something todo ..

    return { t }
  }
})
</script>

<i18n>
{
  "en": {
    "hello": "Hello i18n in SFC!"
  }
}
</i18n>
