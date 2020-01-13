<template>

  <v-flex xs12 sm10 md8 lg6 xl4>
    <h2
      class="headline mb-3"
      v-t="'person.contact'"
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
              v-t="'core.email'"
            />
            <p class="subtitle-1">{{ personModule.getActivePerson.email || $t('core.na') }}</p>
          </v-flex>

          <v-flex xs12 sm6>
            <p
              class="caption mb-0 grey--text"
              v-t="'core.phone'"
            />
            <p class="subtitle-1">{{ personModule.getActivePerson.phone || $t('core.na') }}</p>
          </v-flex>

        </v-layout>
      </v-card-text>

      <!-- //* UPDATE -->
      <v-card-text v-else>
        <v-form
          ref="form"
          v-model="valid"
        >

          <p v-t="'person.emailDescription'" />
          <v-text-field
            v-model="email"
            :rules="emailRules"
            :label="$t('core.email')"
            filled
            required
          />

          <p v-t="'person.phoneDescription'" />
          <v-text-field
            v-model="phone"
            :rules="phoneRules"
            :label="$t('core.phone')"
            filled
            required
          />

        </v-form>
      </v-card-text>

      <!-- //* ACTIONS -->
      <v-card-actions>
        <v-spacer />
        <v-btn
          text
          v-if="!editMode"
          @click.stop="toggleEditMode"
          v-t="'core.edit'"
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
import { Person, Gender } from '../models'

@Component
export default class PersonalContact extends Vue {
  private personModule: PersonModule = getModule(PersonModule, this.$store)

  private valid: any = false
  private editMode: boolean = false
  private loading: boolean = false

  private email: string = this.personModule.getActivePerson?.email || ''
  private emailRules: any[] = [
    (v: string) => !v || /.+@.+\..+/.test(v) || i18n.t('core.emailInvalid'),
  ]
  private phone: string = this.personModule.getActivePerson?.phone || ''
  private phoneRules: any[] = [
    (v: string) => v.length <= 40 || i18n.t('core.fieldMax', { count: 40 }),
  ]

  private toggleEditMode() {
    this.editMode = !this.editMode

    if (this.editMode) {
      this.email = this.personModule.getActivePerson?.email || ''
      this.phone = this.personModule.getActivePerson?.phone || ''
    }
  }

  private async save() {
    const form: any = this.$refs.form
    const personId = this.personModule.getActivePerson?.id || ''

    if (form.validate()) {
      this.loading = true

      await this.personModule.updatePersonContact({
        email: this.email,
        phone: this.phone,
      })

      this.loading = false
      this.toggleEditMode()
    }
  }
}
</script>
