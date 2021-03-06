<template>
  <v-flex v-if="canView" xs12>
    <v-card flat class="ma-2 mb-5">
      <v-card-text class="text--primary">
        <h2 class="headline">{{ $tc('project.congregation.congregations', 2) }}</h2>
      </v-card-text>

      <v-card-actions>
        <FilterCongregationsDialog :filter="filter" />
        <v-spacer />
        <CreateCongregationDialog />
      </v-card-actions>

      <v-data-table
        v-if="congregationModule.getCongregations"
        :loading="loading"
        :headers="headers"
        :items="congregationModule.getCongregations"
        :items-per-page="15"
        :search="filter.search"
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
import FilterCongregationsDialog from '../components/dialogs/FilterCongregationsDialog.vue'
import CreateCongregationDialog from '../components/dialogs/CreateCongregationDialog.vue'
import UpdateCongregationDialog from '../components/dialogs/UpdateCongregationDialog.vue'

@Component({
  components: {
    FilterCongregationsDialog,
    CreateCongregationDialog,
    UpdateCongregationDialog,
  },
})
export default class CongregationsList extends Vue {
  private personModule = getModule(PersonModule, this.$store)
  private congregationModule = getModule(CongregationModule, this.$store)

  private loading = true
  private filter: { search: string } = { search: '' }

  private headers: any[] = [
    { text: 'Name', value: 'name' },
    { text: 'Number', value: 'number' },
    { text: 'Participants', value: 'numberOfParticipants', filterable: false },
    { text: 'Actions', value: 'action', sortable: false, filterable: false },
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
