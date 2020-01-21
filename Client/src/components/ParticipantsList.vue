<template>
  <v-flex
    v-if="canView"
    xs12
  >
    <h2 class="headline mb-3">
      {{ $tc('project.participant.participants', 2) }}
    </h2>
    <v-card
      flat
      class="ma-2 mb-5"
    >
      <v-card-actions>
        <FilterParticipantsDialog :filter="filter" />
      </v-card-actions>
      <v-data-table
        v-if="projectModule.getActiveProject"
        :loading="loading"
        :headers="headers"
        :items="projectModule.getActiveProject.getParticipations"
        :items-per-page="15"
        :search="filter.search"
      >
        <template
          v-if="canEdit"
          v-slot:item.action="{ item }"
        >
          <UpdateParticipantDialog :participation="item" />
        </template>
      </v-data-table>
    </v-card>
  </v-flex>
</template>

<script lang="ts">
import { Vue, Component, Watch } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import PersonModule from '../store/persons'
import ProjectModule from '../store/projects'
import ParticipantModule from '../store/participants'
import UpdateParticipantDialog from './dialogs/UpdateParticipantDialog.vue'
import FilterParticipantsDialog from './dialogs/FilterParticipantsDialog.vue'
import { Person, ParticipationStatus, Gender } from '../models'

@Component({
  components: {
    FilterParticipantsDialog,
    UpdateParticipantDialog,
  }
})
export default class ParticipantsList extends Vue {
  private personModule = getModule(PersonModule, this.$store)
  private projectModule = getModule(ProjectModule, this.$store)
  private participantModule = getModule(ParticipantModule, this.$store)

  private loading: boolean = true
  private filter: { search: string } = { search: '' }

  private headers: any[] = [
    { text: 'First name', value: 'person.firstname' },
    { text: 'Last name', value: 'person.lastname' },
    { text: 'Role', value: 'role.name', filterable: false },
    { text: 'Actions', value: 'action', sortable: false, filterable: false },
  ]

  public get canView() {
    return this.personModule.getActiveRole?.participantsRead === true
  }

  private get canEdit() {
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

    await this.participantModule.loadParticipants()

    this.loading = false
  }

}
</script>
