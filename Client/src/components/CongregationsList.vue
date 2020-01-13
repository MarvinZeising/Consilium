<template>
  <v-flex
    v-if="canView"
    xs12
  >
    <h2 class="headline mb-3">
      {{ $tc('project.congregation.congregations', 2) }}
    </h2>
    <v-card
      flat
      class="ma-2 mb-5"
    >
      <v-card-actions>
        <v-btn
          text
          v-t="'project.congregation.filter'"
        />
        <v-spacer />
        <CreateCongregationDialog />
      </v-card-actions>
      <v-data-table
        v-if="congregationModule.getCongregations"
        :loading="loading"
        :headers="headers"
        :items="congregationModule.getCongregations"
        :items-per-page="15"
      >
        <template v-slot:item.action="{ item }">
          <UpdateCongregationDialog :congregation="item" />
        </template>
      </v-data-table>
    </v-card>
  </v-flex>
</template>

<script lang="ts">
import { Vue, Component, Watch } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import moment from 'moment'
import PersonModule from '../store/persons'
import CongregationModule from '../store/congregations'
import { Person, ParticipationStatus, Gender } from '../models'
import CreateCongregationDialog from '../components/dialogs/CreateCongregationDialog.vue'
import UpdateCongregationDialog from '../components/dialogs/UpdateCongregationDialog.vue'

@Component({
  components: {
    CreateCongregationDialog,
    UpdateCongregationDialog,
  }
})
export default class CongregationsList extends Vue {
  private personModule: PersonModule = getModule(PersonModule, this.$store)
  private congregationModule: CongregationModule = getModule(CongregationModule, this.$store)

  private loading: boolean = true

  private headers: any[] = [
    { text: 'Name', value: 'name' },
    { text: 'Number', value: 'number' },
    { text: 'Participants', value: 'numberOfParticipants' },
    { text: 'Actions', value: 'action', sortable: false },
  ]

  public get canView() {
    return this.personModule.getActiveRole?.participantsWrite === true
  }

  @Watch('personModule.getActivePerson')
  private async onPersonChanged(val: string, oldVal: string) {
    if (this.canView) {
      await this.init()
    }
  }

  @Watch('$route')
  private async onRouteChanged(val: string, oldVal: string) {
    if (this.canView) {
      await this.init()
    }
  }

  private async created() {
    if (this.canView) {
      await this.init()
    }
  }

  private async init() {
    this.loading = true

    await this.congregationModule.loadCongregations()

    this.loading = false
  }

}
</script>
