<template>
    <div class="card" v-if="node != null">
        <div class="card-header" id="headingOne">
        <h2 class="mb-0">
            <button
                class="btn dropdown-toggle btn-block text-left"
                type="button"
                v-on:click="showBody = !showBody"
                >
            {{ node[0] }}
            </button>
        </h2>
        </div>
        <div
            v-if="node[1]"
            v-show="showBody"
            >
            <div class="card-body" v-if="typeof node[1].entries == 'function'">
                <Tree v-bind:nodes="node[1].entries()"></Tree>
            </div>
            <div class="card-body leaf" v-else>
                <table class="table table-striped">
                    <thead>
                        <th>Field</th>
                        <th>Value</th>
                    </thead>
                    <tbody>
                        <tr v-bind:key="key" v-for="(value, key) in node[1]">
                            <td>{{ key }}</td>
                            <td>{{ value }}</td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
    </div>
</template>

<script src="@/components/Tree/TreeNode/TreeNode.js"></script>

<style lang="sass" scoped src="@/components/Tree/TreeNode/TreeNode.sass"></style>
