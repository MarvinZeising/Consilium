<template>
  <v-flex
    v-if="canView"
    xs12 sm10 md8 lg6 xl4
  >
    <h2
      class="headline mb-3"
      v-t="'project.requests'"
    />
    <v-card
      flat
      class="ma-2 mb-5"
      :loading="loading"
    >
      <v-card-text
        class="grey--text"
        v-t="'project.requestsDescription'"
      />
      <v-list v-if="projectModule.getActiveProject.getRequests">
        <v-list-item v-if="projectModule.getActiveProject.getRequests.length === 0">
          <span v-t="'project.noRequests'" />
        </v-list-item>
        <v-list-item
          v-for="(participation, index) in projectModule.getActiveProject.getRequests"
          :key="index"
          two-line
        >
          <v-list-item-content>
            <v-list-item-content>
              <v-list-item-title v-text="participation.person.fullName()" />
              <v-list-item-subtitle v-text="getCreationText(participation.createdTime)" />
            </v-list-item-content>
          </v-list-item-content>

          <v-list-item-action>
            <v-btn
              text
              class="mt-2"
              v-t="'core.handle'"
            />
          </v-list-item-action>
        </v-list-item>
      </v-list>
      <v-card-actions>
        <v-switch
          v-model="requestsAllowed"
          :label="$t(`project.requests${requestsAllowed ? '' : 'Not'}Allowed`)"
          class="mt-0 mb-3 ml-3"
          color="primary"
          inset
          hide-details
        />
        <v-btn
          v-if="requestsAllowed !== initialRequestsAllowed"
          class="ml-3 mb-2"
          v-t="'core.save'"
          @click.stop="saveRequestsAllowed"
          :loading="saving"
        />
      </v-card-actions>
    </v-card>
  </v-flex>
</template>

<script lang="ts">
import { Vue, Component, Watch } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import i18n from '../i18n'
import UserModule from '../store/users'
import PersonModule from '../store/persons'
import ProjectModule from '../store/projects'
import RequestModule from '../store/requests'
import { Person, ParticipationStatus, Gender } from '../models/definitions'
import moment from 'moment'

@Component
export default class ParticipantsRequests extends Vue {
  private userModule: UserModule = getModule(UserModule, this.$store)
  private personModule: PersonModule = getModule(PersonModule, this.$store)
  private projectModule: ProjectModule = getModule(ProjectModule, this.$store)
  private requestModule: RequestModule = getModule(RequestModule, this.$store)

  private loading: boolean = true
  private saving: boolean = false
  private requestsAllowed: any = true
  private initialRequestsAllowed: any = true

  private get canView() {
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

    await this.requestModule.loadRequests();

    const project = this.projectModule.getActiveProject
    if (project) {
      this.requestsAllowed = project.allowRequests
      this.initialRequestsAllowed = this.requestsAllowed
    }

    this.loading = false
  }

  private getCreationText(createdTime: string) {
    const user = this.userModule.getUser
    if (user) {
      return this.$t('project.request.requestedOn', {
        date: moment(createdTime).format(`${user?.dateFormat}, ${user?.timeFormat}`)
      })
    }
    return ''
  }

  private async saveRequestsAllowed() {
    this.saving = true

    await this.requestModule.updateRequestability(this.requestsAllowed)
    this.initialRequestsAllowed = this.requestsAllowed

    this.saving = false
  }

}
</script>
