<template>
  <v-container fluid>
    <v-layout wrap>

      <PersonalGeneral />

      <PersonalContact />

      <!--//* Danger Zone -->
      <v-flex xs12 sm10 md8 lg6 xl4>
        <h2
          class="headline mb-3"
          v-t="'core.dangerZone'"
        />
        <v-card
          v-if="personModule.getActivePerson"
          outlined
          class="ma-2 mb-5"
          style="border-color:#f00;"
        >
          <v-card-text>
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
import DeletePersonDialog from '../../components/dialogs/DeletePersonDialog.vue'

@Component({
  components: {
    PersonalGeneral,
    PersonalContact,
    DeletePersonDialog,
  }
})
export default class Personal extends Vue {
  private personModule: PersonModule = getModule(PersonModule, this.$store)
}
</script>
