<template>
  <v-container>
    <v-layout wrap>

      <!--//* General -->
      <v-flex xs12 sm10 md8 lg6>
        <h2
          class="headline mb-3"
          v-t="'account.general'"
        />
        <v-card
          flat
          class="ma-2 mb-5"
        >
          <v-card-text v-if="userModule.myUser">
            <p
              class="caption mb-0 grey--text"
              v-t="'core.id'"
            />
            <p class="subheading grey--text">
              {{ userModule.myUser.id }}
            </p>

            <p
              class="caption mb-0 grey--text"
              v-t="'core.email'"
            />
            <p class="subheading">
              {{ userModule.myUser.email }}
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

      <!--//* Localization -->
      <v-flex xs12 sm10 md8 lg6>
        <h2
          class="headline mb-3"
          v-t="'account.localization'"
        />
        <v-card flat class="ma-2 mb-5">
          <v-card-text v-if="userModule.myUser">
            <p
              class="caption mb-0 grey--text"
              v-t="'account.language'"
            />
            <p
              class="subheading"
              v-t="'language.' + userModule.myUser.language"
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

      <!--//* Persons -->
      <MyPersons />

      <!--//* Critical Area -->
      <v-flex xs12 sm10 md8 lg6>
        <h2
          class="headline mb-3 error--text"
          v-t="'core.criticalArea'"
        />
        <v-card
          flat
          dark
          color="red lighten-4"
          class="ma-2"
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
import { Project } from '../../models/definitions'
import MyPersons from '../../components/MyPersons.vue'
import DeleteAccountDialog from '../../components/dialogs/DeleteAccountDialog.vue'
import UserModule from '../../store/modules/users'

@Component({
  components: { DeleteAccountDialog, MyPersons }
})
export default class Account extends Vue {
  private userModule: UserModule = getModule(UserModule, this.$store)
}
</script>
