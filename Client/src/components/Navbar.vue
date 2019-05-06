<template>
  <nav>
    <v-toolbar app flat>
      <v-toolbar-side-icon @click="drawer = !drawer"></v-toolbar-side-icon>
      <v-toolbar-title>Consilium</v-toolbar-title>
    </v-toolbar>

    <v-navigation-drawer
      app
      v-model='drawer'
    >
      <v-list :expand="true">

        <NavbarSignedIn v-if="isSignedIn" />

        <NavbarSignedOut v-if="!isSignedIn" />

      </v-list>
    </v-navigation-drawer>

  </nav>
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
import NavbarSignedIn from './NavbarSignedIn.vue'
import NavbarSignedOut from './NavbarSignedOut.vue'

@Component({
  components: {
    NavbarSignedIn,
    NavbarSignedOut
  }
})
export default class Navbar extends Vue {
  private userModule: UserModule = getModule(UserModule, this.$store)

  private drawer: boolean = true

  private async created() {
    this.$router.onReady(() => {
      if (!this.isSignedIn) {
        const currentRoute = this.$router.currentRoute
        const authUnawareRoutes: string[] = [
          'signIn',
          'signUp'
        ]

        if (!authUnawareRoutes.includes(currentRoute.name || '')) {
          this.$router.replace({ name: 'signIn', query: { afterSignIn: currentRoute.fullPath } })
        }
      }
    })
  }

  private get isSignedIn(): boolean {
    return this.userModule.isSignedIn
  }

}
</script>
