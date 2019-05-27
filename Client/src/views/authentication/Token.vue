<template>
  <div class="ma-4" style="word-wrap:break-word; max-width:400px;">
    {{this.token}}
  </div>
</template>

<script lang="ts">
import Vue from 'vue'
import Component from 'vue-class-component'

@Component
export default class Token extends Vue {
  private get token() {
    const userItem = localStorage.getItem('user')
    if (userItem) {
      const user = JSON.parse(userItem)
      return user.token
    }
    return ''
  }
}
</script>
