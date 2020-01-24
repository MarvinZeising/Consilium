<template>

  <v-flex xs12 sm10 md8 lg6 xl4>
    <h2
      class="headline mb-3"
      v-t="'person.theocratic'"
    />
    <v-card
      v-if="personModule.getActivePerson"
      flat
      class="ma-2 mb-5"
    >

      <!-- //* READ -->
      <v-card-text
        v-if="!editMode"
        class="text--primary"
      >
        <v-layout wrap>

          <v-flex xs12 sm6>
            <p
              class="caption mb-0 grey--text"
              v-t="'person.privilege.privilege'"
            />
            <p class="subtitle-1">
              {{ $t('person.privilege.' + personModule.getActivePerson.privilege) }}
            </p>
          </v-flex>

          <v-flex xs12 sm6>
            <p
              class="caption mb-0 grey--text"
              v-t="'person.assignment.assignment'"
            />
            <p class="subtitle-1">
              {{ $t('person.assignment.' + personModule.getActivePerson.assignment) }}
            </p>
          </v-flex>

          <v-flex xs12 sm6 sm->
            <p class="caption mb-0 grey--text">
              {{ $tc('project.congregation.congregations', 1) }}
            </p>
            <p
              v-if="personModule.getActivePerson.congregation"
              class="subtitle-1"
            >
              {{ personModule.getActivePerson.congregation.name }}
            </p>
            <p
              v-else
              v-t="'core.na'"
            />
          </v-flex>

        </v-layout>
      </v-card-text>

      <!-- //* UPDATE -->
      <v-card-text v-else>
        <v-form
          ref="form"
          v-model="valid"
        >

          <p v-t="'person.privilege.description'" />
          <v-select
            v-model="privilege"
            :items="privilegeValues"
            item-text="name"
            item-value="value"
            :label="$t('person.privilege.privilege')"
            filled
            required
          >
            <template v-slot:selection="{ item }">
              <span>{{ $t('person.privilege.' + item.value) }}</span>
            </template>
            <template v-slot:item="{ item }">
              <span>{{ $t('person.privilege.' + item.value) }}</span>
            </template>
          </v-select>

          <p v-t="'person.assignment.description'" />
          <v-select
            v-model="assignment"
            :items="assignmentValues"
            item-text="name"
            item-value="value"
            :label="$t('person.assignment.assignment')"
            filled
            required
          >
            <template v-slot:selection="{ item }">
              <span>{{ $t('person.assignment.' + item.value) }}</span>
            </template>
            <template v-slot:item="{ item }">
              <span>{{ $t('person.assignment.' + item.value) }}</span>
            </template>
          </v-select>

          <p v-t="'person.congregationDescription'" />
          <v-overflow-btn
            v-model="congregationId"
            :items="congregationValues"
            :label="$tc('project.congregation.congregations', 1)"
            item-text="name"
            item-value="id"
            editable
            filled
            :loading="congregationLoading"
            autocomplete="off"
          />

        </v-form>
      </v-card-text>

      <!-- //* ACTIONS -->
      <v-card-actions>
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
          :loading="loading"
          :disabled="!valid"
          @click.stop="save"
        />
      </v-card-actions>
    </v-card>
  </v-flex>

</template>

<script lang="ts">
import { Vue, Component, Watch, Prop } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import i18n from '../i18n'
import PersonModule from '../store/persons'
import CongregationModule from '../store/congregations'
import { Person, Gender, Assignment, Privilege, Congregation } from '../models'

@Component
export default class PersonalTheocratic extends Vue {
  private personModule = getModule(PersonModule, this.$store)
  private congregationModule = getModule(CongregationModule, this.$store)

  private valid: any = false
  private editMode: boolean = false
  private loading: boolean = false
  private congregationLoading = true

  private privilege?: Privilege
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

  private assignment?: Assignment
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

  private congregationId?: string
  private congregationValues: any[] = []

  private async created() {
  }

  private async toggleEditMode() {
    this.editMode = !this.editMode

    if (this.editMode) {
      this.privilege = this.personModule.getActivePerson?.privilege || Privilege.Publisher
      this.assignment = this.personModule.getActivePerson?.assignment || Assignment.Publisher
      this.congregationId = this.personModule.getActivePerson?.congregation?.id

      this.congregationLoading = true

      await this.congregationModule.loadCongregations()
      this.congregationValues = this.congregationModule.getCongregations

      this.congregationLoading = false
    }
  }

  private async save() {
    const form: any = this.$refs.form
    const personId = this.personModule.getActivePerson?.id || ''

    if (form.validate() && this.privilege && this.assignment) {
      this.loading = true

      await this.personModule.updatePersonTheocratic({
        privilege: this.privilege,
        assignment: this.assignment,
        congregationId: this.congregationId,
      })

      this.loading = false
      this.toggleEditMode()
    }
  }
}
</script>
