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
      <!-- <v-card-title>
        <v-spacer></v-spacer>
        <v-text-field
          v-model="search"
          append-icon="search"
          label="Search"
          single-line
          hide-details
        />
      </v-card-title> -->
      <v-data-table
        v-if="projectModule.getActiveProject"
        :loading="loading"
        :headers="headers"
        :items="projectModule.getActiveProject.getParticipations"
        :items-per-page="15"
        :search="search"
      >
        <template
          v-if="canEdit"
          v-slot:item.action="{ item }"
        >
          <DeleteParticipantDialog
            v-if="item.person.id !== personModule.getActivePersonId"
            :participationId="item.id"
          />
        </template>
      </v-data-table>
    </v-card>
  </v-flex>
</template>

<script lang="ts">
import { Vue, Component, Watch } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import moment from 'moment'
import UserModule from '../store/users'
import PersonModule from '../store/persons'
import ProjectModule from '../store/projects'
import ParticipantModule from '../store/participants'
import DeleteParticipantDialog from './dialogs/DeleteParticipantDialog.vue'
import { Person, ParticipationStatus, Gender } from '../models'

@Component({
  components: {
    DeleteParticipantDialog,
  }
})
export default class ParticipantsList extends Vue {
  private userModule: UserModule = getModule(UserModule, this.$store)
  private personModule: PersonModule = getModule(PersonModule, this.$store)
  private projectModule: ProjectModule = getModule(ProjectModule, this.$store)
  private participantModule: ParticipantModule = getModule(ParticipantModule, this.$store)

  private loading: boolean = true
  private search: string = ''

  private headers: any[] = [
    { text: 'First name', value: 'person.firstname' },
    { text: 'Last name', value: 'person.lastname' },
    { text: 'Role', value: 'role.name' },
    { text: 'Actions', value: 'action', sortable: false },
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
