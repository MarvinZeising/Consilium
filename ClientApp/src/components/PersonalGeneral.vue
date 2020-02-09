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

          <v-flex xs12>
            <p
              class="caption mb-0 grey--text"
              v-t="'core.id'"
            />
            <p class="subtitle-1 grey--text">{{ personModule.getActivePersonId }}</p>
          </v-flex>
          <v-flex xs12 sm6>
            <p
              class="caption mb-0 grey--text"
              v-t="'person.firstname'"
            />
            <p class="subtitle-1">{{ personModule.getActivePerson.firstname }}</p>
          </v-flex>
          <v-flex xs12 sm6>
            <p
              class="caption mb-0 grey--text"
              v-t="'person.lastname'"
            />
            <p class="subtitle-1">{{ personModule.getActivePerson.lastname }}</p>
          </v-flex>
          <v-flex xs12>
            <p
              class="caption mb-0 grey--text"
              v-t="'person.gender.gender'"
            />
            <p
              class="subtitle-1"
              v-t="'person.gender.' + personModule.getActivePerson.gender"
            />
          </v-flex>

          <v-flex xs12 sm6>
            <p
              class="caption mb-0 grey--text"
              v-t="'core.createdTime'"
            />
            <p class="subtitle-1 grey--text">
              {{ userModule.getUser.formatDateTime(personModule.getActivePerson.createdTime) }}
            </p>
          </v-flex>

          <v-flex xs12 sm6>
            <p
              class="caption mb-0 grey--text"
              v-t="'core.lastUpdatedTime'"
            />
            <p class="subtitle-1 grey--text">
              {{ userModule.getUser.formatDateTime(personModule.getActivePerson.lastUpdatedTime) }}
            </p>
          </v-flex>

        </v-layout>
      </v-card-text>

      <!-- //* UPDATE -->
      <v-card-text v-else>
        <v-form
          ref="form"
          v-model="valid"
        >

          <p v-t="'person.firstnameDescription'" />
          <v-text-field
            v-model="firstname"
            :label="$t('person.firstname')"
            :rules="nameRules"
            :counter="firstname.length >= 30 ? '40' : false"
            filled
            required
          />

          <p v-t="'person.lastnameDescription'" />
          <v-text-field
            v-model="lastname"
            :label="$t('person.lastname')"
            :rules="nameRules"
            :counter="lastname.length >= 30 ? '40' : false"
            filled
            required
          />

          <p v-t="'person.gender.description'" />
          <v-select
            v-model="gender"
            :items="genderValues"
            item-text="name"
            item-value="value"
            :label="$t('person.gender.gender')"
            filled
            required
          >
            <template v-slot:selection="{ item }">
              <span>{{ $t('person.gender.' + item.value) }}</span>
            </template>
            <template v-slot:item="{ item }">
              <span>{{ $t('person.gender.' + item.value) }}</span>
            </template>
          </v-select>

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
import { Person, Gender } from '../models'
import UserModule from '../store/users'
import PersonModule from '../store/persons'

@Component
export default class PersonalGeneral extends Vue {
  private userModule = getModule(UserModule, this.$store)
  private personModule = getModule(PersonModule, this.$store)

  private valid = false
  private editMode = false
  private loading = false

  private firstname: string = this.personModule.getActivePerson?.firstname || ''
  private lastname: string = this.personModule.getActivePerson?.lastname || ''
  private nameRules: any[] = [
    (v: string) => !!v || i18n.t('core.fieldRequired'),
    (v: string) => v.length <= 40 || i18n.t('core.fieldMax', { count: 40 }),
    (v: string) => v.trim().length >= 2 || i18n.t('core.fieldMin', { count: 2 }),
  ]
  private gender: string = this.personModule.getActivePerson?.gender || 'male'
  private genderValues: any[] = [ 'male', 'female' ].map((value) => {
    return { value }
  })

  private toggleEditMode() {
    this.editMode = !this.editMode

    if (this.editMode) {
      this.firstname = this.personModule.getActivePerson?.firstname || ''
      this.lastname = this.personModule.getActivePerson?.lastname || ''
      this.gender = this.personModule.getActivePerson?.gender || ''
    }
  }

  private async save() {
    const form: any = this.$refs.form
    const personId = this.personModule.getActivePerson?.id || ''

    if (form.validate()) {
      this.loading = true

      await this.personModule.updatePersonGeneral({
        id: personId,
        firstname: this.firstname,
        lastname: this.lastname,
        gender: this.gender,
      })

      this.toggleEditMode()
    }
  }
}
</script>
