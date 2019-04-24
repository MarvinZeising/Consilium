<template>
  <v-footer>
    <v-spacer></v-spacer>
    <router-link
      :to="{ name: 'termsOfUse' }"
      style="text-decoration:none"
    >
      Terms of Use
    </router-link>
    <span class="pl-2 pr-2">|</span>
    <router-link
      :to="{ name: 'privacyPolicy' }"
      style="text-decoration:none"
    >
      Privacy Policy
    </router-link>
    <span class="pl-2 pr-2">|</span>
    <span class="pr-3">
      0.0.1
    </span>
  </v-footer>
</template>

<script lang="ts">
import Vue from 'vue'
export default Vue.extend({})
</script>
