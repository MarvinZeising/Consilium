<template>

  <v-flex xs12 sm10 md8 lg6 xl4>
    <h2
      class="headline mb-3"
      v-t="'person.miscellaneous'"
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
              v-t="'person.languages'"
            />
            <p class="subtitle-1">{{ personModule.getActivePerson.languages || $t('core.na') }}</p>
          </v-flex>

          <v-flex xs12>
            <p
              class="caption mb-0 grey--text"
              v-t="'person.notes'"
            />
            <p
              class="subtitle-1"
              v-html="toHtmlBreaks(personModule.getActivePerson.notes) || $t('core.na')"
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

          <p v-t="'person.languages'" />
          <v-text-field
            v-model="languages"
            :label="$t('person.languages')"
            :rules="languageRules"
            :counter="languages.length >= 80 ? '100' : false"
            filled
          />

          <p v-t="'person.notes'" />
          <v-textarea
            v-model="notes"
            :label="$t('person.notes')"
            :rules="notesRules"
            :counter="notes.length >= 900 ? '1000' : false"
            filled
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
import { Person, Gender } from '../models'

@Component
export default class PersonalMisc extends Vue {
  private personModule: PersonModule = getModule(PersonModule, this.$store)

  private valid: any = false
  private editMode: boolean = false
  private loading: boolean = false

  private languages: string = this.personModule.getActivePerson?.firstname || ''
  private notes: string = this.personModule.getActivePerson?.lastname || ''

  private toggleEditMode() {
    this.editMode = !this.editMode

    if (this.editMode) {
      this.languages = this.personModule.getActivePerson?.languages || ''
      this.notes = this.personModule.getActivePerson?.notes || ''
    }
  }

  private async save() {
    const form: any = this.$refs.form

    if (form.validate()) {
      this.loading = true

      await this.personModule.updatePersonMiscellaneous({
        languages: this.languages,
        notes: this.notes,
      })

      this.loading = false
      this.toggleEditMode()
    }
  }

  private toHtmlBreaks(content: string) {
    return content.replace(/(\r\n|\n|\r)/gm, '<br>')
  }
}
</script>
