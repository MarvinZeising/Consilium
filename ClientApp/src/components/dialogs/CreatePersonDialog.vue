<template>
  <v-dialog v-model="dialog" max-width="1000px">
    <template v-slot:activator="{ on }">
      <v-btn v-on="on" text v-t="'person.create'" @click="opened" />
    </template>
    <v-card>
      <v-form v-model="valid" ref="form">
        <v-toolbar flat color="accent">
          <v-toolbar-title v-t="'person.create'" />
          <v-spacer />
          <v-btn icon class="mr-0" @click="showHelp = !showHelp">
            <v-icon v-if="showHelp">help_outline</v-icon>
            <v-icon v-else>help</v-icon>
          </v-btn>
        </v-toolbar>
        <v-card-text v-if="showHelp">
          <i class="subtitle-1">
            {{ $t('person.createDescription1') }}
            <br />
            {{ $t('person.createDescription2') }}
            <br />
            {{ $t('person.createDescription3') }}
          </i>
        </v-card-text>
        <v-card-text class="pa-2">
          <v-layout wrap>
            <TextControl
              :model="firstnameModel"
              label="person.firstname"
              :description="showHelp ? 'person.firstnameDescription' : ''"
              :minLength="0"
              :maxLength="40"
              required
            />

            <TextControl
              :model="lastnameModel"
              label="person.lastname"
              :description="showHelp ? 'person.lastnameDescription' : ''"
              :minLength="0"
              :maxLength="40"
              required
            />

            <SelectControl
              :model="genderModel"
              label="person.gender.gender"
              :values="['male', 'female']"
              :description="showHelp ? 'person.gender.description' : ''"
              itemString="person.gender."
            />
          </v-layout>
        </v-card-text>

        <v-divider />

        <v-card-actions>
          <v-spacer />
          <v-btn text v-t="'core.cancel'" @click.stop="dialog = false" />
          <v-btn
            text
            type="submit"
            color="primary"
            v-t="'core.save'"
            :loading="loading"
            :disabled="!valid"
            @click.prevent="save"
          />
        </v-card-actions>
      </v-form>
    </v-card>
  </v-dialog>
</template>

<script lang="ts">
import { Vue, Component } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import i18n from '../../i18n'
import PersonModule from '../../store/persons'
import TextControl from '../controls/TextControl.vue'
import CreatePerson from '../../views/account/CreatePerson.vue'
import SelectControl from '../controls/SelectControl.vue'

@Component({
  components: {
    TextControl,
    SelectControl,
  },
})
export default class CreatePersonDialog extends Vue {
  private personModule = getModule(PersonModule, this.$store)

  private dialog = false
  private valid: any = null
  private loading = false
  private showHelp = false

  private firstnameModel = { value: '' }
  private lastnameModel = { value: '' }
  private genderModel = { value: 'male' }

  private opened() {}

  private async save() {
    if (this.valid) {
      this.loading = true

      await this.personModule.createPerson({
        firstname: this.firstnameModel.value,
        lastname: this.lastnameModel.value,
        gender: this.genderModel.value,
      })

      this.loading = false
      this.dialog = false
    }
  }
}
</script>
