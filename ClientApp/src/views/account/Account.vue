<template>
  <v-container fluid>
    <v-layout wrap>
      <AccountGeneral />

      <AccountInterface />

      <AccountPersons />

      <v-flex xs12 sm10 md8 lg6 xl4>
        <v-card outlined class="ma-2" style="border-color:#f00;">
          <v-card-text class="text--primary">
            <h2 class="headline mb-5" v-t="'core.dangerZone'" />
            <v-layout column>
              <UpdatePasswordDialog />

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
import { Project } from '../../models'
import AccountGeneral from '../../components/AccountGeneral.vue'
import AccountInterface from '../../components/AccountInterface.vue'
import AccountPersons from '../../components/AccountPersons.vue'
import DeleteAccountDialog from '../../components/dialogs/DeleteAccountDialog.vue'
import UpdatePasswordDialog from '../../components/dialogs/UpdatePasswordDialog.vue'
import UserModule from '../../store/users'

@Component({
  components: {
    AccountGeneral,
    AccountInterface,
    DeleteAccountDialog,
    AccountPersons,
    UpdatePasswordDialog,
  },
})
export default class Account extends Vue {
  private userModule = getModule(UserModule, this.$store)
}
</script>
