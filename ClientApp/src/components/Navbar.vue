<template>
  <div>
    <v-navigation-drawer app v-model="drawer" :clipped="$vuetify.breakpoint.lgAndUp">
      <NavbarSignedIn v-if="userModule.getUser" />
      <NavbarSignedOut v-else />

      <template v-slot:append>
        <v-divider />
        <div class="text-center pa-2 caption" v-text="commitHash" />
      </template>
    </v-navigation-drawer>

    <v-app-bar app :clipped-left="$vuetify.breakpoint.lgAndUp" color="navbar" dark>
      <v-app-bar-nav-icon @click.stop="drawer = !drawer" />
      <v-toolbar-title>Consilium</v-toolbar-title>

      <v-spacer />

      <div v-if="this.userModule.getUser">
        <v-menu v-model="accountMenu" left bottom offset-y>
          <template v-slot:activator="{ on }">
            <v-btn v-on="on" icon>
              <v-icon>account_circle</v-icon>
            </v-btn>
          </template>
          <v-card style="min-width:300px; text-align:center;">
            <v-card-text class="text-center">
              <div class="mb-3">
                <span class="subtitle-2" v-t="'account.signedInWith'" />
                <br />
                <span class="subtitle-1 text--primary" v-text="userModule.getUser.email" />
              </div>
              <v-btn
                outlined
                rounded
                :to="{ name: 'account' }"
                style="border-color:grey;"
                v-t="'account.manage'"
              />
            </v-card-text>
            <v-divider />
            <v-card-actions class="d-flex justify-center">
              <v-btn
                text
                @click.stop="signOut"
                style="border-color:grey lighten-2;"
                v-t="'account.signOut'"
              />
            </v-card-actions>
            <v-divider />
            <v-card-text>
              <v-btn text x-small :to="{ name: 'termsOfUse' }" v-t="'core.termsOfUse'" />
              <v-icon x-small>fiber_manual_record</v-icon>
              <v-btn text x-small :to="{ name: 'privacyPolicy' }" v-t="'core.privacyPolicy'" />
            </v-card-text>
          </v-card>
        </v-menu>
      </div>

      <v-menu v-if="!this.userModule.getUser" v-model="languageMenu" left bottom offset-y>
        <template v-slot:activator="{ on }">
          <v-toolbar-items style="margin-right:-16px;">
            <v-btn text v-on="on">
              <span v-t="'language.' + $i18n.locale" />
              <v-icon>arrow_drop_down</v-icon>
            </v-btn>
          </v-toolbar-items>
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
import { Vue, Component, Watch } from 'vue-property-decorator'
import { mapGetters, mapActions } from 'vuex'
import { getModule } from 'vuex-module-decorators'
import { Project } from '../models'
import ProjectModule from '../store/projects'
import UserModule from '../store/users'
import NavbarSignedIn from './NavbarSignedIn.vue'
import NavbarSignedOut from './NavbarSignedOut.vue'
import axios from 'axios';

@Component({
  components: {
    NavbarSignedIn,
    NavbarSignedOut,
  },
})
export default class Navbar extends Vue {
  private userModule = getModule(UserModule, this.$store)
  private projectModule = getModule(ProjectModule, this.$store)

  private drawer = false
  private accountMenu = false
  private languageMenu = false

  private commitHash = 'development'

  private language: string = ''
  private languages: string[] = []

  @Watch('$route')
  private async onRouteChanged(val: string, oldVal: string) {
    await this.init()
  }

  private async created() {
    await this.init()
  }

  private async init() {
    this.drawer = this.$vuetify.breakpoint.lgAndUp

    this.language = this.$i18n.locale
    this.languages = this.$i18n.availableLocales

    this.accountMenu = false
    this.languageMenu = false

    if (process.env.NODE_ENV !== 'development') {
      const response = await axios.get('/version')
      this.commitHash = response.data
    }
  }

  private updateLanguage(language: string) {
    this.language = language
    this.$i18n.locale = this.language
  }

  private signOut() {
    this.accountMenu = false
    this.$router.push({ name: 'signOut' })
  }
}
</script>
