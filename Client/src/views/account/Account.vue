<template>
  <v-container>
    <v-layout wrap>

      <AccountGeneral />

      <AccountInterface />

      <!--//* Persons -->
      <AccountPersons />

      <!--//* Danger Zone -->
      <v-flex xs12 sm10 md8 lg6 xl4>
        <h2
          class="headline mb-3"
          v-t="'core.dangerZone'"
        />
        <v-card
          outlined
          class="ma-2"
          style="border-color:#f00;"
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
import AccountGeneral from '../../components/AccountGeneral.vue'
import AccountInterface from '../../components/AccountInterface.vue'
import AccountPersons from '../../components/AccountPersons.vue'
import DeleteAccountDialog from '../../components/dialogs/DeleteAccountDialog.vue'
import UserModule from '../../store/modules/users'

@Component({
  components: {
    AccountGeneral,
    AccountInterface,
    DeleteAccountDialog,
    AccountPersons,
  }
})
export default class Account extends Vue {
  private userModule: UserModule = getModule(UserModule, this.$store)
}
</script>
