<template>

  <v-flex xs12 sm10 md8 lg6 xl4>
    <h2
      class="headline mb-3"
      v-t="'core.general'"
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

          <v-flex xs6>
            <p
              class="caption mb-0 grey--text"
              v-t="'person.firstname'"
            />
            <p class="subtitle-1">{{ personModule.getActivePerson.firstname }}</p>
          </v-flex>

          <v-flex xs6>
            <p
              class="caption mb-0 grey--text"
              v-t="'person.lastname'"
            />
            <p class="subtitle-1">{{ personModule.getActivePerson.lastname }}</p>
          </v-flex>

          <v-flex xs6>
            <p
              class="caption mb-0 grey--text"
              v-t="'person.gender'"
            />
            <p
              class="subtitle-1"
              v-t="'person.' + personModule.getActivePerson.gender"
            />
          </v-flex>

        </v-layout>
      </v-card-text>

      <!-- //* UPDATE -->
      <v-card-text v-else>
        <v-form ref="form">
          <p v-t="'person.firstnameDescription'" />
          <v-text-field
            v-model="firstname"
            :label="$t('person.firstname')"
            :rules="nameRules"
            counter="20"
            filled
            required
          />

          <p v-t="'person.lastnameDescription'" />
          <v-text-field
            v-model="lastname"
            :label="$t('person.lastname')"
            :rules="nameRules"
            counter="20"
            filled
            required
          />

          <p v-t="'person.genderDescription'" />
          <v-select
            v-model="gender"
            :items="genderValues"
            item-text="name"
            item-value="value"
            :label="$t('person.gender')"
            filled
            required
          ></v-select>
        </v-form>
      </v-card-text>

      <!-- //* ACTIONS -->
      <v-card-actions>
        <v-spacer />
        <v-btn
          text
          v-if="!editMode"
          @click="toggleEditMode"
          v-t="'core.edit'"
        />
        <v-btn
          text
          v-if="editMode"
          @click="toggleEditMode"
          v-t="'core.cancel'"
        />
        <v-btn
          text
          v-if="editMode"
          color="primary"
          @click="save"
          v-t="'core.save'"
        />
      </v-card-actions>
    </v-card>
  </v-flex>

</template>

<script lang="ts">
import { Vue, Component, Watch, Prop } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import { VForm } from 'vuetify/lib'
import i18n from '../i18n'
import { Person, Gender } from '../models/definitions'
import PersonModule from '../store/persons'

@Component
export default class PersonalGeneral extends Vue {
  private personModule: PersonModule = getModule(PersonModule, this.$store)

  private editMode: boolean = false

  private toggleEditMode() {
    this.editMode = !this.editMode

    if (this.editMode) {
      this.firstname = this.personModule.getActivePerson?.firstname || ''
      this.lastname = this.personModule.getActivePerson?.lastname || ''
      this.gender = this.personModule.getActivePerson?.gender || ''
    }
  }

  private firstname: string = this.personModule.getActivePerson?.firstname || ''
  private lastname: string = this.personModule.getActivePerson?.lastname || ''
  private nameRules: any[] = [
    (v: string) => !!v || i18n.t('core.fieldRequired'),
    (v: string) => v.length <= 20 || i18n.t('core.fieldMax', { count: 20 }),
    (v: string) => v.length >= 2 || i18n.t('core.fieldMin', { count: 2 })
  ]
  private gender: string = this.personModule.getActivePerson?.gender || 'male'
  private genderValues: any[] = [
    { value: 'male', name: i18n.t('person.male') },
    { value: 'female', name: i18n.t('person.female') },
  ]

  private async save() {
    const form: any = this.$refs.form
    const personId = this.personModule.getActivePerson?.id || ''

    if (form.validate()) {
      await this.personModule.updatePersonGeneral({
        id: personId,
        firstname: this.firstname,
        lastname: this.lastname,
        gender: this.gender,
      })
      // TODO: add error handling

      this.toggleEditMode()
    }
  }
}
</script>
