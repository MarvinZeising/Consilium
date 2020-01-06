<template>
  <v-flex xs12>
    <h2 class="headline mb-3">
      {{ $tc('project.participant.participants', 2) }}
    </h2>
    <v-card
      flat
      class="ma-2 mb-5"
    >
      <v-data-table
        :headers="headers"
        :items="participantModule.getParticipations"
        :items-per-page="15"
        :loading="loading"
      >
        <template v-slot:item.action="{ item }">
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
import ParticipantModule from '../store/participants'
import DeleteParticipantDialog from './dialogs/DeleteParticipantDialog.vue'
import { Person, ParticipationStatus, Gender } from '../models/definitions'

@Component({
  components: {
    DeleteParticipantDialog,
  }
})
export default class ParticipantsList extends Vue {
  private userModule: UserModule = getModule(UserModule, this.$store)
  private personModule: PersonModule = getModule(PersonModule, this.$store)
  private participantModule: ParticipantModule = getModule(ParticipantModule, this.$store)

  private loading: boolean = true

  private headers: any[] = [
    { text: 'First name', value: 'person.firstname' },
    { text: 'Last name', value: 'person.lastname' },
    { text: 'Role', value: 'role.name' },
    { text: 'Actions', value: 'action', sortable: false },
  ]

  @Watch('$route')
  private async onRouteChanged(val: string, oldVal: string) {
    await this.init()
  }

  private async created() {
    this.init()
  }

  private async init() {
    await this.participantModule.loadParticipants()

    this.loading = false
  }

}
</script>
