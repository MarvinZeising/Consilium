<template>
  <v-container fluid>
    <v-layout wrap>

      <!--//* General -->
      <v-flex xs12>
        <h2 class="headline mb-3">General</h2>
      </v-flex>
      <v-flex
        xs12 sm10 md8 lg6
        class="mb-5 pa-2"
      >
        <v-card flat>
          <v-card-text>
            <p class="caption mb-0 grey--text">
              ID
            </p>
            <p class="subheading grey--text">
              {{ id }}
            </p>

            <p class="caption mb-0 grey--text">
              Email
            </p>
            <p class="subheading">
              {{ email }}
            </p>
          </v-card-text>

          <v-card-actions>
            <v-spacer></v-spacer>
            <v-btn
              flat
              :to="{ name: 'updateAccountGeneral' }"
            >
              Edit
            </v-btn>
          </v-card-actions>
        </v-card>
      </v-flex>

      <!--//* Language -->
      <v-flex xs12>
        <h2 class="headline mb-3">Language</h2>
      </v-flex>
      <v-flex
        xs12 sm10 md8 lg6
        class="mb-5 pa-2"
      >
        <v-card flat>
          <v-card-text>
            <p class="caption mb-0 grey--text">
              Interface language
            </p>
            <p class="subheading">
              {{ displayLanguage(language) }}
            </p>
          </v-card-text>

          <v-card-actions>
            <v-spacer></v-spacer>
            <v-btn
              flat
              :to="{ name: 'updateAccountLanguage' }"
            >
              Edit
            </v-btn>
          </v-card-actions>
        </v-card>
      </v-flex>

      <!--//* Critical Area -->
      <v-flex xs12>
        <h2 class="headline mb-3 error--text">Critical area</h2>
      </v-flex>
      <v-flex
        xs12 sm10 md8 lg6
        class="mb-5 pa-2"
      >
        <v-card
          flat
          dark
          color="red lighten-4"
        >
          <v-card-text>
            <v-btn
              :to="{ name: 'updateAccountPassword' }"
              class="grey lighten-3 black--text mb-4"
            >
              Change my password
            </v-btn>

            <DeleteAccountDialog />
          </v-card-text>
        </v-card>
      </v-flex>

    </v-layout>
  </v-container>
</template>

<script lang="ts">
import Vue from 'vue'
import Component from 'vue-class-component'
import { getModule } from 'vuex-module-decorators'
import { Project } from '@/models/definitions'
import DeleteAccountDialog from '@/components/dialogs/DeleteAccountDialog.vue'
import UserModule from '@/store/modules/users'

@Component({
  components: { DeleteAccountDialog }
})
export default class Account extends Vue {
  private userModule: UserModule = getModule(UserModule, this.$store)

  private id: string = ''
  private email: string = ''
  private language: string = ''

  private created() {
    const user = this.userModule.myUser
    if (user) {
      this.id = user.id
      this.email = user.email
      this.language = user.language
    }
  }

  private displayLanguage(language: string) {
    switch (language) {
      case 'en-US':
        return 'English (US)'
        break
      case 'de-DE':
        return 'German'
        break
      default:
        return `Unknown language: ${language}`
    }
  }
}
</script>
