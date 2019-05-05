<template>
  <div>

    <!--//* Sign in -->
    <v-list-tile :to="{ name: 'signIn' }">
      <v-list-tile-title>Sign in to your account</v-list-tile-title>
    </v-list-tile>

    <!--//* Sign up -->
    <v-list-tile :to="{ name: 'signUp' }">
      <v-list-tile-title>Create an account</v-list-tile-title>
    </v-list-tile>

  </div>
</template>

<script lang="ts">
import Vue from 'vue'
import axios from '@/tools/axios'
import { mapGetters, mapActions } from 'vuex'
import { Project } from '@/models/definitions'
import { getModule } from 'vuex-module-decorators'
import ProjectModule from '@/store/modules/projects'
import UserModule from '@/store/modules/users'
import Component from 'vue-class-component'

@Component
export default class Navbar extends Vue {

  private drawer: boolean = false

}
</script>
