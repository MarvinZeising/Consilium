<template>
  <v-dialog
    v-model="dialog"
    max-width="1000px"
  >
    <template v-slot:activator="{ on }">
      <v-btn
        v-on="on"
        icon
      >
        <v-icon>edit</v-icon>
      </v-btn>
    </template>
    <v-card>
      <v-form
        v-model="valid"
        ref="form"
      >
        <v-card-title>
          <span
            class="headline"
            v-t="'project.participant.update'"
          />
        </v-card-title>
        <v-card-text>
          <span
            class="subtitle-1"
            v-t="'project.participant.updateDescription'"
          />
        </v-card-text>
        <v-card-text class="pa-0">
          <v-layout wrap>

            <v-flex
              xs12 sm6 md4
              class="pa-4"
            >
              <v-text-field
                v-model="firstname"
                :rules="nameRules"
                :label="$t('person.firstname')"
                :counter="firstname.length >= 30 ? 40 : false"
                filled
                required
              />
            </v-flex>

            <v-flex
              xs12 sm6 md4
              class="pa-4"
            >
              <v-text-field
                v-model="lastname"
                :rules="nameRules"
                :label="$t('person.lastname')"
                :counter="lastname.length >= 30 ? 40 : false"
                filled
                required
              />
            </v-flex>

            <v-flex
              xs12 sm6 md4
              class="pa-4"
            >
              <v-select
                v-model="gender"
                :items="genderValues"
                item-text="name"
                item-value="value"
                :label="$t('person.gender.gender')"
                filled
              >
                <template v-slot:selection="{ item }">
                  <span>{{ $t('person.gender.' + item.value) }}</span>
                </template>
                <template v-slot:item="{ item }">
                  <span>{{ $t('person.gender.' + item.value) }}</span>
                </template>
              </v-select>
            </v-flex>

            <v-flex
              xs12 sm6 md4
              class="pa-4"
            >
              <v-text-field
                v-model="email"
                :rules="emailRules"
                :label="$t('core.email')"
                filled
              />
            </v-flex>

            <v-flex
              xs12 sm6 md4
              class="pa-4"
            >
              <v-select
                v-model="language"
                :items="languageValues"
                item-text="name"
                item-value="value"
                :label="$t('language.language')"
                filled
                required
              >
                <template v-slot:selection="{ item }">
                  <span>{{ $t('language.' + item.value) }}</span>
                </template>
                <template v-slot:item="{ item }">
                  <span>{{ $t('language.' + item.value) }}</span>
                </template>
              </v-select>
            </v-flex>

            <v-flex
              xs12 sm6 md4
              class="pa-4"
            >
              <v-text-field
                v-model="phone"
                :rules="phoneRules"
                :label="$t('core.phone')"
                filled
              />
            </v-flex>

            <v-flex
              xs12 sm6 md4
              class="pa-4"
            >
              <v-select
                v-model="privilege"
                :items="privilegeValues"
                item-text="name"
                item-value="value"
                :label="$t('person.privilege.privilege')"
                filled
              >
                <template v-slot:selection="{ item }">
                  <span>{{ $t('person.privilege.' + item.value) }}</span>
                </template>
                <template v-slot:item="{ item }">
                  <span>{{ $t('person.privilege.' + item.value) }}</span>
                </template>
              </v-select>
            </v-flex>

            <v-flex
              xs12 sm6 md4
              class="pa-4"
            >
              <v-select
                v-model="assignment"
                :items="assignmentValues"
                item-text="name"
                item-value="value"
                :label="$t('person.assignment.assignment')"
                filled
              >
                <template v-slot:selection="{ item }">
                  <span>{{ $t('person.assignment.' + item.value) }}</span>
                </template>
                <template v-slot:item="{ item }">
                  <span>{{ $t('person.assignment.' + item.value) }}</span>
                </template>
              </v-select>
            </v-flex>

            <v-flex
              xs12 sm6 md4
              class="pa-4"
            >
              <v-text-field
                v-model="languages"
                :label="$t('person.languages')"
                :rules="languagesRules"
                :counter="languages.length >= 80 ? '100' : false"
                filled
              />
            </v-flex>

            <v-flex
              xs12 sm6 md4
              class="pa-4"
            >
              <v-textarea
                v-model="notes"
                :label="$t('person.notes')"
                :rules="notesRules"
                :counter="notes.length >= 900 ? '1000' : false"
                auto-grow
                filled
              />
            </v-flex>

          </v-layout>
        </v-card-text>
        <v-card-actions>
          <v-btn
            text
            v-t="'core.cancel'"
            @click.stop="dialog = false"
          />
          <v-spacer />
          <DeleteParticipantDialog
            v-if="participation.personId !== personModule.getActivePersonId"
            :participationId="participation.id"
          />
          <v-btn
            text
            type="submit"
            color="primary"
            v-t="'core.save'"
            :loading="loading"
            :disabled="!valid"
            @click.stop="save"
          />
        </v-card-actions>
      </v-form>
    </v-card>
  </v-dialog>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import i18n from '../../i18n'
import PersonModule from '../../store/persons'
import ProjectModule from '../../store/projects'
import ParticipantModule from '../../store/participants'
import DeleteParticipantDialog from '../../components/dialogs/DeleteParticipantDialog.vue'
import { Article, Person, Participation, Gender, Privilege, Assignment, Language } from '../../models'

@Component({
  components: {
    DeleteParticipantDialog,
  }
})
export default class UpdateParticipantDialog extends Vue {
  private personModule: PersonModule = getModule(PersonModule, this.$store)
  private projectModule: ProjectModule = getModule(ProjectModule, this.$store)
  private participantModule: ParticipantModule = getModule(ParticipantModule, this.$store)

  @Prop(Participation)
  private readonly participation?: Participation

  private dialog: any = false
  private valid: any = null
  private loading: boolean = false

  private firstname?: string
  private lastname?: string
  private gender?: Gender
  private email?: string
  private language?: Language
  private phone?: string
  private privilege?: Privilege
  private assignment?: Assignment
  private languages?: string
  private notes?: string

  private nameRules: any[] = [
    (v: string) => !!v || i18n.t('core.fieldRequired'),
    (v: string) => v.length >= 3 || i18n.t('core.fieldMin', { count: 3 }),
    (v: string) => v.length <= 100 || i18n.t('core.fieldMax', { count: 40 }),
  ]
  private emailRules: any[] = [
    (v: string) => !v || /.+@.+\..+/.test(v) || i18n.t('core.emailInvalid'),
  ]
  private phoneRules: any[] = [
    (v: string) => v.length <= 40 || i18n.t('core.fieldMax', { count: 40 }),
  ]
  private languagesRules: any[] = [
    (v: string) => v.length <= 100 || i18n.t('core.fieldMax', { count: 100 }),
  ]
  private notesRules: any[] = [
    (v: string) => v.length <= 1000 || i18n.t('core.fieldMax', { count: 1000 }),
  ]

  private genderValues: any[] = [ 'male', 'female' ].map((value) => {
    return { value }
  })
  private languageValues: any[] = i18n.availableLocales.map((value) => {
    return { value }
  })
  private privilegeValues: any[] = [
    'publisher',
    'auxiliary',
    'regular',
    'special',
    'circuit',
    'bethelite',
    'construction'
  ].map((value) => {
    return { value }
  })
  private assignmentValues: any[] = [
    'publisher',
    'ministerial',
    'elder',
    'cobe',
    'secretary',
    'serviceOverseer'
  ].map((value) => {
    return { value }
  })

  private created() {
    if (this.participation) {
      this.firstname = this.participation.person?.firstname || ''
      this.lastname = this.participation.person?.lastname || ''
      this.gender = this.participation.person?.gender || Gender.Male
      this.email = this.participation.person?.email || ''
      this.language = this.participation.person?.language || Language.enUS
      this.phone = this.participation.person?.phone || ''
      this.privilege = this.participation.person?.privilege || Privilege.Publisher
      this.assignment = this.participation.person?.assignment || Assignment.Publisher
      this.languages = this.participation.person?.languages || ''
      this.notes = this.participation.person?.notes || ''
    }
  }

  private async save() {
    if (this.participation) {
      this.loading = true

      await this.participantModule.updateParticipant({
        participationId: this.participation.id,
        firstname: this.firstname || '',
        lastname: this.lastname || '',
        gender: this.gender || Gender.Male,
        email: this.email || '',
        language: this.language || Language.enUS,
        phone: this.phone || '',
        privilege: this.privilege || Privilege.Publisher,
        assignment: this.assignment || Assignment.Publisher,
        languages: this.languages || '',
        notes: this.notes || '',
      })

      this.loading = false
      this.dialog = false
    }
  }

}
</script>
