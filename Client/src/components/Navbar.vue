<template>
  <div>

    <NavbarSignedIn
      v-if="isSignedIn"
      :drawer="drawer"
    />
    <NavbarSignedOut
      v-else
      :drawer="drawer"
    />

    <v-app-bar
      app
      :clipped-left="$vuetify.breakpoint.lgAndUp"
      color="primary"
      dark
    >
      <v-app-bar-nav-icon @click.stop="drawer = !drawer" />
      <v-toolbar-title>Consilium</v-toolbar-title>
      <v-spacer />
      <v-menu v-if="!isSignedIn">
        <template v-slot:activator="{ on }">
          <v-toolbar-title v-on="on">
            <span v-t="'language.' + language" />
            <v-icon>arrow_drop_down</v-icon>
          </v-toolbar-title>
        </template>
        <v-list>
          <v-list-item
            v-for="language in languages"
            :key="language"
            @click="updateLanguage(language)"
          >
            <v-list-item-title v-t="'language.' + language" />
          </v-list-item>
        </v-list>
      </v-menu>
    </v-app-bar>

  </div>
</template>

<script lang="ts">
import { Vue, Component } from 'vue-property-decorator'
import { mapGetters, mapActions } from 'vuex'
import { getModule } from 'vuex-module-decorators'
import { Project } from '@/models/definitions'
import ProjectModule from '@/store/modules/projects'
import UserModule from '@/store/modules/users'
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
