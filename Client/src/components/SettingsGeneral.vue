<template>

  <v-flex
    v-if="canView"
    xs12 sm10 md8 lg6 xl4
  >
    <h2
      class="headline mb-3"
      v-t="'core.general'"
    />
    <v-card
      :loading="loading"
      flat
      class="ma-2 mb-5"
    >

      <!-- //* READ -->
      <v-card-text
        v-if="!editMode"
        class="text--primary"
      >
        <v-layout
          v-if="projectModule.getActiveProject"
          wrap
        >

          <v-flex xs12>
            <p
              class="caption mb-0 grey--text"
              v-t="'core.id'"
            />
            <p class="subtitle-1 grey--text">{{ projectModule.getActiveProject.id }}</p>
          </v-flex>

          <v-flex xs6>
            <p
              class="caption mb-0 grey--text"
              v-t="'core.name'"
            />
            <p class="subtitle-1">{{ projectModule.getActiveProject.name }}</p>
          </v-flex>

          <v-flex xs6>
            <p
              class="caption mb-0 grey--text"
              v-t="'core.email'"
            />
            <p class="subtitle-1">{{ projectModule.getActiveProject.email }}</p>
          </v-flex>

          <v-flex xs6>
            <p
              class="caption mb-0 grey--text"
              v-t="'core.createdTime'"
            />
            <p class="subtitle-1 grey--text">
              {{ userModule.getUser.formatDateTime(projectModule.getActiveProject.createdTime) }}
            </p>
          </v-flex>

          <v-flex xs6>
            <p
              class="caption mb-0 grey--text"
              v-t="'core.lastUpdatedTime'"
            />
            <p class="subtitle-1 grey--text">
              {{ userModule.getUser.formatDateTime(projectModule.getActiveProject.lastUpdatedTime) }}
            </p>
          </v-flex>

        </v-layout>
      </v-card-text>

      <!-- //* UPDATE -->
      <v-card-text v-else>
        <v-form ref="form">
          <p v-t="'project.nameDescription'" />
          <v-text-field
            v-model="name"
            :label="$t('core.name')"
            :rules="nameRules"
            counter="40"
            filled
            required
          />
          <p v-t="'project.emailDescription'" />
          <v-text-field
            v-model="email"
            :label="$t('core.email')"
            :rules="emailRules"
            filled
            required
          />
        </v-form>
      </v-card-text>

      <!-- //* ACTIONS -->
      <v-card-actions v-if="canEdit">
        <v-spacer />
        <v-btn
          v-if="!editMode"
          text
          v-t="'core.edit'"
          @click.stop="toggleEditMode"
        />
        <v-btn
          v-if="editMode"
          text
          v-t="'core.cancel'"
          @click.stop="toggleEditMode"
        />
        <v-btn
          v-if="editMode"
          text
          color="primary"
          v-t="'core.save'"
          @click.stop="save"
          :loading="saving"
        />
      </v-card-actions>
    </v-card>
  </v-flex>

</template>

<script lang="ts">
import { Vue, Component, Watch, Prop } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import moment from 'moment'
import i18n from '../i18n'
import AlertModule from '../store/alerts'
import UserModule from '../store/users'
import PersonModule from '../store/persons'
import ProjectModule from '../store/projects'
import { Exceptions } from '../models'

@Component
export default class SettingsGeneral extends Vue {
  private alertModule: AlertModule = getModule(AlertModule, this.$store)
  private userModule: UserModule = getModule(UserModule, this.$store)
  private personModule: PersonModule = getModule(PersonModule, this.$store)
  private projectModule: ProjectModule = getModule(ProjectModule, this.$store)

  private loading: boolean = true
  private saving: boolean = false
  private editMode: boolean = false

  private name: string = this.projectModule.getActiveProject?.name || ''
  private nameRules: any[] = [
    (v: string) => !!v || i18n.t('core.fieldRequired'),
    (v: string) => v.length <= 40 || i18n.t('core.fieldMax', { count: 40 }),
    (v: string) => v.length >= 3 || i18n.t('core.fieldMin', { count: 3 }),
    (v: string) => v.charAt(0) === v.charAt(0).toUpperCase() || i18n.t('core.fieldCamelCase')
  ]
  private email: string = this.projectModule.getActiveProject?.email || ''
  private emailRules: any[] = [
    (v: string) => !!v || i18n.t('core.fieldRequired'),
    (v: string) => /.+@.+\..+/.test(v) || i18n.t('core.emailInvalid')
  ]

  private get canView() {
    return this.personModule.getActiveRole?.settingsRead === true
  }

  private get canEdit() {
    return this.personModule.getActiveRole?.settingsWrite === true
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

    const projectId = this.$route.params.projectId
    const personId = this.personModule.getActivePersonId
    if (personId) {
      await this.projectModule.loadProject({
        projectId,
        personId,
      })
    }

    this.loading = false
  }

  private toggleEditMode() {
    this.editMode = !this.editMode

    if (this.editMode) {
      this.name = this.projectModule.getActiveProject?.name || ''
      this.email = this.projectModule.getActiveProject?.email || ''
    }
  }

  private async save() {
    const form: any = this.$refs.form
    const userName = this.projectModule.getActiveProject?.name || ''

    if (form.validate()) {
      this.saving = true

      const response = await this.projectModule.updateProjectGeneral({
        name: this.name,
        email: this.email
      })
      if (response === Exceptions.ProjectNameUnique) {
        const thisName = this.name
        this.nameRules.push((v: string) => v !== thisName || i18n.t('project.nameUnique'))
        form.validate()
      } else if (response) {
        this.alertModule.showAlert({
          message: i18n.t('core.generalError').toString(),
          color: 'error',
          timeout: 5000,
        })
      } else {
        this.toggleEditMode()
      }

      // TODO: check that email is correct
      // TODO: maybe by sending an email to verify the new one

      this.saving = false
    }
  }

}
</script>
