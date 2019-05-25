<template>
  <div>

    <!--//* Sign in -->
    <v-list-tile :to="{ name: 'signIn' }">
      <v-list-tile-title>{{ $t('navbar.signIn') }}</v-list-tile-title>
    </v-list-tile>

    <!--//* Sign up -->
    <v-list-tile :to="{ name: 'signUp' }">
      <v-list-tile-title>{{ $t('navbar.signUp') }}</v-list-tile-title>
    </v-list-tile>

  </div>
</template>

<script lang="ts">
import Vue from 'vue'
import Component from 'vue-class-component'

@Component
export default class NavbarSignedOut extends Vue {
  private drawer: boolean = false
}
</script>
