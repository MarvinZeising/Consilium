<template>
  <v-container fluid>
    <v-layout wrap>
      <PersonalGeneral />

      <PersonalContact />

      <PersonalTheocratic />

      <PersonalMiscellaneous />

      <v-flex xs12 sm10 md8 lg6 xl4>
        <v-card
          v-if="personModule.getActivePerson"
          outlined
          class="ma-2 mb-5"
          style="border-color:#f00;"
        >
          <v-card-text class="text--primary">
            <h2 class="headline mb-5" v-t="'core.dangerZone'" />
            <DeletePersonDialog :personId="personModule.getActivePersonId" />
          </v-card-text>
        </v-card>
      </v-flex>
    </v-layout>
  </v-container>
</template>

<script lang="ts">
import { Vue, Component, Watch, Prop } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import { Person } from '../../models'
import PersonModule from '../../store/persons'
import PersonalGeneral from '../../components/PersonalGeneral.vue'
import PersonalContact from '../../components/PersonalContact.vue'
import PersonalTheocratic from '../../components/PersonalTheocratic.vue'
import PersonalMiscellaneous from '../../components/PersonalMiscellaneous.vue'
import DeletePersonDialog from '../../components/dialogs/DeletePersonDialog.vue'

@Component({
  components: {
    PersonalGeneral,
    PersonalContact,
    PersonalTheocratic,
    PersonalMiscellaneous,
    DeletePersonDialog,
  },
})
export default class Personal extends Vue {
  private personModule = getModule(PersonModule, this.$store)
}
</script>
