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
      <v-card-title>
        <v-text-field
          v-model="search"
          append-icon="search"
          :label="$t('core.search')"
          single-line
          hide-details
        />
      </v-card-title>
      <v-data-table
        v-if="congregationModule.getCongregations"
        :loading="loading"
        :headers="headers"
        :items="congregationModule.getCongregations"
        :items-per-page="15"
        :search="search"
      >
        <template
          v-slot:item.action="{ item }"
        >
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
import CongregationModule from '../store/congregations'
import { Person, ParticipationStatus, Gender } from '../models'

@Component
export default class CongregationsList extends Vue {
  private userModule: UserModule = getModule(UserModule, this.$store)
  private personModule: PersonModule = getModule(PersonModule, this.$store)
  private projectModule: ProjectModule = getModule(ProjectModule, this.$store)
  private congregationModule: CongregationModule = getModule(CongregationModule, this.$store)

  private loading: boolean = true
  private search: string = ''

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
