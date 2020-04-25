<template>
  <v-dialog v-model="dialog" max-width="1000px">
    <template v-slot:activator="{ on }">
      <v-btn v-on="on" text v-t="'shift.category.create'" @click="opened" />
    </template>
    <v-card>
      <v-form v-model="valid" ref="form">
        <v-toolbar flat color="accent">
          <v-toolbar-title v-t="'shift.category.create'" />
          <v-spacer />
          <v-btn icon class="mr-0" @click="showHelp = !showHelp">
            <v-icon v-if="showHelp">help_outline</v-icon>
            <v-icon v-else>help</v-icon>
          </v-btn>
        </v-toolbar>
        <v-card-text v-if="showHelp">
          <i class="subtitle-1" v-t="'shift.category.createDescription'" />
        </v-card-text>
        <v-card-text class="pa-2">
          <NameControl :model="nameModel" translationPath="shift.category.nameDescription" />
        </v-card-text>

        <v-divider />

        <EligibilityControl
          v-for="(eligibility, index) in category.eligibilities"
          :key="index"
          :eligibility="eligibility"
          :showDescription="showHelp"
        />

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
import ProjectModule from '../../store/projects'
import CategoryModule from '../../store/categories'
import NameControl from '../controls/NameControl.vue'
import EligibilityControl from '../controls/EligibilityControl.vue'
import { Category, Eligibility } from '../../models'

@Component({
  components: {
    NameControl,
    EligibilityControl,
  },
})
export default class CreateCategoryDialog extends Vue {
  private projectModule = getModule(ProjectModule, this.$store)
  private categoryModule = getModule(CategoryModule, this.$store)

  private dialog = false
  private valid: any = null
  private loading = false
  private showHelp = false

  private category = Category.create({
    eligibilities: [],
  })
  private nameModel = { value: '' }

  private opened() {
    if (this.projectModule.getActiveProject) {
      this.category.eligibilities = this.projectModule.getActiveProject.roles
        .filter((role) => role.editable)
        .map((role) => {
          return Eligibility.create({
            role,
            roleId: role.id,
            shiftsRead: true,
            shiftsWrite: false,
            isTeamCaptain: true,
            isSubstituteCaptain: false,
          })
        })
    }
  }

  private async save() {
    if (this.valid) {
      this.loading = true

      this.category.name = this.nameModel.value

      await this.categoryModule.createCategory(this.category)

      this.loading = false
      this.dialog = false
    }
  }
}
</script>
