<template>
  <v-container fluid>
    <v-layout wrap>

      <!--//* General -->
      <v-flex xs12>
        <h2
          class="headline mb-3"
          v-t="'account.general'"
        />
      </v-flex>
      <v-flex
        xs12 sm10 md8 lg6
        class="mb-5 pa-2"
      >
        <v-card flat>
          <v-card-text>
            <p
              class="caption mb-0 grey--text"
              v-t="'core.id'"
            />
            <p class="subheading grey--text">
              {{ id }}
            </p>

            <p
              class="caption mb-0 grey--text"
              v-t="'core.email'"
            />
            <p class="subheading">
              {{ email }}
            </p>
          </v-card-text>

          <v-card-actions>
            <v-spacer />
            <v-btn
              flat
              :to="{ name: 'updateAccountGeneral' }"
              v-t="'core.edit'"
            />
          </v-card-actions>
        </v-card>
      </v-flex>

      <!--//* Language -->
      <v-flex xs12>
        <h2
          class="headline mb-3"
          v-t="'account.localization'"
        />
      </v-flex>
      <v-flex
        xs12 sm10 md8 lg6
        class="mb-5 pa-2"
      >
        <v-card flat>
          <v-card-text>
            <p
              class="caption mb-0 grey--text"
              v-t="'account.language'"
            />
            <p
              class="subheading"
              v-t="'language.' + language"
            />
          </v-card-text>

          <v-card-actions>
            <v-spacer />
            <v-btn
              flat
              :to="{ name: 'updateAccountLanguage' }"
              v-t="'core.edit'"
            />
          </v-card-actions>
        </v-card>
      </v-flex>

      <!--//* Critical Area -->
      <v-flex xs12>
        <h2
          class="headline mb-3 error--text"
          v-t="'core.criticalArea'"
        />
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
              v-t="'account.changePassword'"
            />

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
import DeleteAccountDialog from '../../components/dialogs/DeleteAccountDialog.vue'
import UserModule from '../../store/modules/users'

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

}
</script>
