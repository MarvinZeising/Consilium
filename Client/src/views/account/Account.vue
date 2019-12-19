<template>
  <v-container>
    <v-layout wrap>

      <!--//* General -->
      <v-flex xs12 sm10 md8 lg6 xl4>
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
            <p class="subtitle-1 grey--text">
              {{ userModule.myUser.id }}
            </p>

            <p
              class="caption mb-0 grey--text"
              v-t="'core.email'"
            />
            <p class="subtitle-1">
              {{ userModule.myUser.email }}
            </p>
          </v-card-text>

          <v-card-actions>
            <v-spacer />
            <v-btn
              text
              :to="{ name: 'updateAccountGeneral' }"
              v-t="'core.edit'"
            />
          </v-card-actions>
        </v-card>
      </v-flex>

      <!--//* Localization -->
      <v-flex xs12 sm10 md8 lg6 xl4>
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
              class="subtitle-1"
              v-t="'language.' + userModule.myUser.language"
            />
          </v-card-text>

          <v-card-actions>
            <v-spacer />
            <v-btn
              text
              :to="{ name: 'updateAccountLanguage' }"
              v-t="'core.edit'"
            />
          </v-card-actions>
        </v-card>
      </v-flex>

      <!--//* Persons -->
      <MyPersons />

      <!--//* Danger Zone -->
      <v-flex xs12 sm10 md8 lg6 xl4>
        <h2
          class="headline mb-3 error--text"
          v-t="'core.dangerZone'"
        />
        <v-card
          flat
          color="red lighten-4"
          class="ma-2"
        >
          <v-card-text>
            <v-layout column>
              <v-btn
                :to="{ name: 'updateAccountPassword' }"
                class="mb-4"
                v-t="'account.changePassword'"
              />

              <DeleteAccountDialog />
            </v-layout>
          </v-card-text>
        </v-card>
      </v-flex>

    </v-layout>
  </v-container>
</template>

<script lang="ts">
import { Vue, Component } from 'vue-property-decorator'
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
