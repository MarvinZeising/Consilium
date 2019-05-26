<template>
  <v-footer>
    <v-spacer />
    <router-link
      :to="{ name: 'termsOfUse' }"
      style="text-decoration:none"
    >
      {{ $t('navbar.termsOfUse') }}
    </router-link>
    <span class="pl-2 pr-2">|</span>
    <router-link
      :to="{ name: 'privacyPolicy' }"
      style="text-decoration:none"
    >
      {{ $t('navbar.privacyPolicy') }}
    </router-link>
    <span class="pl-2 pr-2">|</span>
    <span class="pr-3">
      0.0.1 DEV
    </span>
  </v-footer>
</template>

<script lang="ts">
import Vue from 'vue'
import Component from 'vue-class-component'

@Component({})
export default class Footer extends Vue {}
</script>
