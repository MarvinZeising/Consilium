<template>
  <nav>
    <v-toolbar app flat>
      <v-toolbar-side-icon @click="drawer = !drawer"></v-toolbar-side-icon>
      <v-toolbar-title>Consilium</v-toolbar-title>
      <v-spacer></v-spacer>
      <v-menu v-if="!isSignedIn">
        <template v-slot:activator="{ on }">
          <v-toolbar-title v-on="on">
            <span v-t="'language.' + language" />
            <v-icon>arrow_drop_down</v-icon>
          </v-toolbar-title>
        </template>

        <v-list>
          <v-list-tile
            v-for="language in languages"
            :key="language"
            @click="updateLanguage(language)"
          >
            <v-list-tile-title v-t="'language.' + language" />
          </v-list-tile>
        </v-list>
      </v-menu>
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
  private projectModule: ProjectModule = getModule(ProjectModule, this.$store)

  private drawer: boolean = true
  private language: string = ''
  private languages: string[] = []

  private async created() {
    this.language = this.$i18n.locale
    this.languages = this.$i18n.availableLocales
  }

  private get isSignedIn(): boolean {
    return this.userModule.isSignedIn
  }

  private updateLanguage(language: string) {
    this.language = language
    this.$i18n.locale = this.language
  }

}
</script>
